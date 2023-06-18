namespace mosaCupBackend.Models.ReqModels
{
    public class PostReq
    {
        public string Uid { get; set; }
        public string Content { get; set; }
        public Guid? ReplyId { get; set; } = null;
    }
}
