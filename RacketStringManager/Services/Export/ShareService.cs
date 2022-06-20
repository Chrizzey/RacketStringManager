namespace RacketStringManager.Services.Export
{
    internal class ShareService : IShare
    {
        public Task RequestAsync(ShareTextRequest request)
        {
            return Share.RequestAsync(request);
        }

        public Task RequestAsync(ShareFileRequest request)
        {
            return Share.RequestAsync(request);
        }

        public Task RequestAsync(ShareMultipleFilesRequest request)
        {
            return Share.RequestAsync(request);
        }
    }
}
