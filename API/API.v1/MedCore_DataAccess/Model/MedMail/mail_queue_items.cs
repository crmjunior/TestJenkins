using System;
using System.Collections.Generic;

namespace MedCore_API.MedMail
{
    public partial class mail_queue_items
    {
        public int mailitem_id { get; set; }
        public string profile_id { get; set; }
        public string recipients { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string body_format { get; set; }
        public int status { get; set; }
        public int RetryCount { get; set; }
        public DateTime date_queued { get; set; }
        public string copy_recipients { get; set; }
        public string blind_copy_recipients { get; set; }
    }
}
