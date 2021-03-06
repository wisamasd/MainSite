﻿using System;
using System.Collections.Generic;
using Application.Dal.Domain.Files;

namespace Application.Dal.Domain.News
{
    public class NewsItem : BaseEntity
    {

        public string Header { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string AutorFio { get; set; }
        public string LastChangeAuthor { get; set; }
        public DateTime LastChangeDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<File> Files { get; set; }

    }
}