const LoadCommData = () => {
    var commentBlock = '';
    let id = window.location.pathname.split('/').pop();
    $.ajax({
        url: '/Home/GetComments',
        method: 'GET',
        success: (result) => {
            console.log(result)
            let comments = result.comments;
            $.each(comments, (k, v) => {                
                if (v.campaignId == id) {
                    let date = moment(v.postDate).format('DD.MM.yyyy H:mm:ss');
                    commentBlock += `<div class="col-8 single-comment">
                                            <div class="row justify-content-between">
                                                <div>
                                                    <time>${date}</time>
                                                    <p>${v.author}</p>
                                                </div>
                                                <form id="del-com-${v.commentId}-form" method="post" action="/Home/DeleteComment"
                                                      data-ajax="true" data-ajax-mode="replace" data-ajax-update="#commentSection" enctype="multipart/form-data">
                                                    <input type="hidden" name="CommentId" value="${v.commentId}"/>
                                                    <input type="hidden" name="CampaignId" value="${v.campaignId}"/>                                                    
                                                </form>
                                            </div>                                            
                                            <div>
                                                ${v.content}
                                            </div>
                                            <hr/>
                                            <form class="d-inline-block" method="post" action="/Home/Like"
                                                                data-ajax="true" data-ajax-mode="replace" data-ajax-update="#commentSection" enctype="multipart/form-data">
                                                    <input type="hidden" name="CommentId" value="${v.commentId}"/>
                                                    <input type="hidden" name="CampaignId" value="${v.campaignId}"/>
                                                    <span>${v.likesCount}</span>
                                                    <button class="like-btn">
                                                        <i class="fa fa-thumbs-o-up" aria-hidden="true"></i>
                                                    </button>                                                   
                                            </form>
                                            <form class="d-inline-block" method="post" action="/Home/Dislike"
                                                                data-ajax="true" data-ajax-mode="replace" data-ajax-update="#commentSection" enctype="multipart/form-data">   
                                                    <input type="hidden" name="CommentId" value="${v.commentId}"/>
                                                    <input type="hidden" name="CampaignId" value="${v.campaignId}"/>
                                                    <span>${v.dislikesCount}</span>
                                                    <button class="like-btn">
                                                        <i class="fa fa-thumbs-o-down" aria-hidden="true"></i>
                                                    </button>
                                            </form>                                            
                                    </div>`         

                }
            });
            let delBtn = `<button class="delete-btn">
                            <i class="fa fa-times" aria-hidden="true"></i>
                          </button>`;
            $("#comments").html(commentBlock);  
            $.each(comments, (k, v) => {
                if (v.campaignId == id) {
                    if (result.username == v.author || result.isModer) {
                        $(`#del-com-${v.commentId}-form`).append(delBtn);
                    }                    
                }
            });            
        },
        error: (error) => {
            console.log('Error:' + error);
        }
    });
}

$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/CommentHub").build();
    connection.on("LoadComments", () => {
        LoadCommData();
    });
    LoadCommData();
    connection.start().catch(err => console.error(err.toString()));
});