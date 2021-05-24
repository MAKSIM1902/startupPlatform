using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Сrowdfunding.CloudStorage;
using System.Threading.Tasks;
using Сrowdfunding.Data;
using Сrowdfunding.Hubs;
using Сrowdfunding.Models;
using Сrowdfunding.Models.ViewModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Сrowdfunding.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private readonly IHubContext<CommentHub> _commentHub;
        private readonly IHubContext<NewsHub> _newsHub;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, IHubContext<CommentHub> commentHub, IHubContext<NewsHub> newsHub)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _commentHub = commentHub;
            _newsHub = newsHub;
        }


        public IActionResult Index()
        {
            var campaigns = _context.Campaigns.ToList();
            var indexVm = new IndexViewModel
            {
                Campaigns = campaigns
            };
            return View(indexVm);
        }


        [HttpGet]
        public IActionResult GetComments()
        {
            var comments = _context.Comments.ToList();
            bool isModer = this.User.IsInRole("Admin") || this.User.IsInRole("Moderator");
            var username = _userManager.GetUserName(this.User);
            return Ok(new { Comments = comments, IsModer = isModer, Username = username });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            _logger.LogInformation(DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss"));
            var campaign = _context.Campaigns.Find(id);
            if (campaign.EndTime < DateTime.Now || campaign.RemainSum == 0)
            {
                campaign.Ended = true;
                _context.SaveChanges();
            } 
            var commentVm = new CommentViewModel
            {
                Campaign = _context.Campaigns.Find(id),
                Comments = _context.Comments.ToList(),
                Rating = new Rating
                {
                    CampaignId = id
                }
            };
            return View(commentVm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DetailsAsync(Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                _logger.LogError("IN DETAILS");
                var username = _userManager.GetUserName(this.User);
                comment.PostDate = DateTime.Now;
                comment.Author = username;
                _logger.LogInformation(comment.CampaignId.ToString());
                _context.Add(comment);
                _context.SaveChanges();
                await _commentHub.Clients.All.SendAsync("LoadComments");
                return RedirectToAction("CommentList", new { id = id });
            }
            return View();
        }

        public PartialViewResult CommentList(int id)
        {
            var commentVm = new CommentViewModel
            {
                Campaign = _context.Campaigns.Find(id),
                Comments = _context.Comments.ToList(),
                Rating = new Rating
                {
                    CampaignId = id
                }
            };
            return PartialView(commentVm);
        }

        [HttpGet]
        public IActionResult GetNews()
        {
            var news = _context.News.ToList();
            bool isModer = this.User.IsInRole("Admin") || this.User.IsInRole("Moderator");
            var username = _userManager.GetUserName(this.User);
            return Ok(new { News = news, IsModer = isModer, Username = username });
        }

        public async Task<IActionResult> AddNewsAsync(News news)
        {
            var username = _userManager.GetUserName(this.User);
            news.Author = username;
            news.PostDate = DateTime.Now;
            _context.Add(news);
            _context.SaveChanges();
            await _newsHub.Clients.All.SendAsync("LoadNews");
            return RedirectToAction("NewsList", new { id = news.CampaignId });
        }

        public PartialViewResult NewsList(int id)
        {
            var newsVm = new NewsViewModel
            {
                Campaign = _context.Campaigns.Find(id),
                News = _context.News.ToList()                
            };
            return PartialView(newsVm);
        }

        [HttpGet]
        public IActionResult Rewards(int id)
        {
            var campaign = _context.Campaigns.Find(id);
            return View(campaign);
        }

        [HttpPost]
        public IActionResult Rewards(Reward reward)
        {
            _context.Add(reward);
            _context.SaveChanges();
            var username = _userManager.GetUserName(this.User);
            var commentVm = new CommentViewModel
            {
                Campaign = _context.Campaigns.Find(reward.CampaignId),
                Comments = _context.Comments.ToList(),
                Rating = new Rating
                {
                    CampaignId = reward.CampaignId,
                    Username = username
                }
            };
            return View("Details", commentVm);
        }

        [HttpGet]
        public IActionResult Support(int id)
        {
            var supportVm = new SupportViewModel
            {
                Campaign = _context.Campaigns.Find(id),
                Rewards = _context.Rewards.ToList()
            };
            
            return View(supportVm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Support(Reward reward, int id)
        {
            var campaign = _context.Campaigns.Find(id);
            var rew = _context.Rewards.Find(reward.RewardId);
            if (campaign.RemainSum <= reward.Price)
            {
                campaign.RemainSum = 0;
            }
            else
            {
                campaign.RemainSum -= reward.Price;
            }
            if (rew.Count > 0)
            {
                rew.Count--;
            }            
            _context.SaveChanges();
            var supportVm = new SupportViewModel
            {
                Campaign = campaign,
                Rewards = _context.Rewards.ToList()
            };
            return View(supportVm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rate(Rating rating)
        {
            var username = _userManager.GetUserName(this.User);
            var ratings = _context.Ratings.Where(rate => rate.Username == username && rate.CampaignId == rating.CampaignId).ToList();
            if (ratings.Count() == 0)
            {
                rating.Username = username;
                _context.Add(rating);
                _context.SaveChanges();
            }
            return RedirectToAction("_RatingPartial", new { id = rating.RateId });
        }

        public PartialViewResult _RatingPartial(int id)
        {
            return PartialView(_context.Ratings.Find(id));
        }

        public bool CheckLikes(Comment comment, string username)
        {
            return _context.Likes
                    .Where(like => like.CommentId == comment.CommentId)
                    .Where(like => like.Username == username)
                    .Any();
        }

        public bool CheckDislikes(Comment comment, string username)
        {
            return _context.Dislikes
                    .Where(dislike => dislike.CommentId == comment.CommentId)
                    .Where(dislike => dislike.Username == username)
                    .Any();
        }

        public void DecreaseLikes(Comment comment, string username, Comment dbComment)
        {
            var like = _context.Likes
                   .Where(l => l.CommentId == comment.CommentId)
                   .Where(l => l.Username == username)
                   .FirstOrDefault();
            _context.Likes.Remove(like);
            dbComment.LikesCount--;
        }

        public void DecreaseDislikes(Comment comment, string username, Comment dbComment)
        {
            var dislike = _context.Dislikes
                       .Where(d => d.CommentId == comment.CommentId)
                       .Where(d => d.Username == username)
                       .FirstOrDefault();
            _context.Dislikes.Remove(dislike);
            dbComment.DislikesCount--;
        }

        [Authorize]
        public IActionResult Like(Comment comment)
        {
            _logger.LogError("like:" + comment.CommentId.ToString() + "; " + comment.CampaignId.ToString());
            var username = _userManager.GetUserName(this.User);
            var dbComment = _context.Comments.Find(comment.CommentId);
            var newLike = new Like
            {
                CommentId = comment.CommentId,
                Username = username
            };
            if (CheckLikes(comment, username))
            {
                DecreaseLikes(comment, username, dbComment);
            }
            else
            {
                if (CheckDislikes(comment, username))
                {
                    DecreaseDislikes(comment, username, dbComment);
                }
                _context.Likes.Add(newLike);
                dbComment.LikesCount++;
            }
            _context.SaveChanges();
            return RedirectToAction("CommentList", new { id = comment.CampaignId });
        }

        [Authorize]
        public IActionResult Dislike(Comment comment)
        {
            var username = _userManager.GetUserName(this.User);
            var dbComment = _context.Comments.Find(comment.CommentId);
            var newDislike = new Dislike
            {
                CommentId = comment.CommentId,
                Username = username
            };
            if (CheckDislikes(comment, username))
            {
                DecreaseDislikes(comment, username, dbComment);
            }
            else
            {
                if (CheckLikes(comment, username))
                {
                    DecreaseLikes(comment, username, dbComment);
                }
                _context.Dislikes.Add(newDislike);
                dbComment.DislikesCount++;
            }
            _context.SaveChanges();
            return RedirectToAction("CommentList", new { id = comment.CampaignId });
        }

        public IActionResult DeleteComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Comments.Remove(_context.Comments.Find(comment.CommentId));
                _context.SaveChanges();
            }
            return RedirectToAction("CommentList", new { id = comment.CampaignId });
        }

        public IActionResult DeleteNews(News news)
        {
            if (ModelState.IsValid)
            {
                _context.News.Remove(_context.News.Find(news.NewsId));
                _context.SaveChanges();
            }
            return RedirectToAction("NewsList", new { id = news.CampaignId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
