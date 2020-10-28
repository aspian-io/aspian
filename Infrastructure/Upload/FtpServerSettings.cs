namespace Infrastructure.Upload
{
    public class FtpServerSettings
    {
        public string ServerUri { get; set; }
        public int ServerPort { get; set; }
        public string ServerUsername { get; set; }
        public string ServerPassword { get; set; }
        public bool IsActive { get; set; }
    }
}