const LoadNewsData = () => {
    var newsBlock = '';
    let id = window.location.pathname.split('/').pop();
    $.ajax({
        url: '/Home/GetNews',
        method: 'GET',
        success: (result) => {
            console.log(result)
            let news = result.news;
            $.each(news, (k, v) => {
                if (v.campaignId == id) {
                    let date = moment(v.postDate).format('DD.MM.yyyy H:mm:ss');
                    newsBlock += `<div class="col-8">
                                            <div class="row justify-content-between">
                                                <div>
                                                    <time>${date}</time>
                                                    <p>${v.author}</p>
                                                </div>
                                                <form id="del-news-${v.newsId}-form" method="post" action="/Home/DeleteNews"
                                                      data-ajax="true" data-ajax-mode="replace" data-ajax-update="#newsSection" enctype="multipart/form-data">
                                                    <input type="hidden" name="NewsId" value="${v.newsId}"/>
                                                    <input type="hidden" name="CampaignId" value="${v.campaignId}"/>                                                    
                                                </form>
                                            </div>                                            
                                            <div>
                                                <h3>${v.title}</h3>
                                                ${v.newsContent}
                                            </div>
                                            <hr/>                                          
                                    </div>`

                }
            });
            let delBtn = `<button class="delete-btn">
                            <i class="fa fa-times" aria-hidden="true"></i>
                          </button>`;
            $("#news").html(newsBlock);
            $.each(news, (k, v) => {
                if (v.campaignId == id) {
                    if (result.username == v.author || result.isModer) {
                        $(`#del-news-${v.newsId}-form`).append(delBtn);
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
    let connection = new signalR.HubConnectionBuilder().withUrl("/NewsHub").build();
    connection.on("LoadNews", () => {
        LoadNewsData();
    });
    LoadNewsData();
    connection.start().catch(err => console.error(err.toString()));
});