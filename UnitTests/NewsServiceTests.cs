﻿using System;
using System.Collections.Generic;
using System.Text;
using Application.Dal;
using Application.Dal.Domain.News;
using Application.Services.News;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class NewsServiceTests
    {
        [TestCase("")]
        [TestCase("00000000-0000-0000-0000-000000000000")]
        public void GetNewsItem_RequestWithInvalidId_ReturnNull(string id)
        {
            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();

            NewsService newsService = new NewsService(mock.Object);
            NewsItem result = newsService.GetNewsItem(id);

            Assert.IsTrue(result == null);
        }

        [Test]
        public void GetNewsItem_RequestOneExistNewsItem_ReturnNewsItem()
        {
            string guid = Guid.NewGuid().ToString();

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.Get(guid)).Returns(new NewsItem()
            {
                Id = guid,
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
            });

            NewsService newsService = new NewsService(mock.Object);
            NewsItem result = newsService.GetNewsItem(guid);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Id == guid);
        }

        [Test]
        public void GetNewsItem_RequestOneNotExistNewsItem_ReturnNull()
        {
            string guid = Guid.NewGuid().ToString();

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.Get(guid)).Returns(new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
            });

            NewsService newsService = new NewsService(mock.Object);
            NewsItem result = newsService.GetNewsItem(Guid.NewGuid().ToString());

            Assert.IsTrue(result == null);
        }

        [Test]
        public void GetNewsItem_RequestAllItem_ReturnAllItems()
        {
            var listItems = new List<NewsItem>()
            {
                new NewsItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = null,
                    Description = "Description1",
                    Files = null,
                    Header = "Header1",
                },
                new NewsItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = null,
                    Description = "Description2",
                    Files = null,
                    Header = "Header2",
                },
                new NewsItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = null,
                    Description = "Description2",
                    Files = null,
                    Header = "Header2",
                }
            };
            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll()).Returns(listItems);

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = (List<NewsItem>)newsService.GetNewsItem(authorId: null, category: null);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == listItems.Count);
        }

        [Test]
        public void GetNewsItem_RequestManyItem_ReturnAllItemsAndOldestFirst()
        {
            DateTime data2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime data2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime data2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data2021,
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2019,
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2020,
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll()).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = (List<NewsItem>)newsService.GetNewsItem(authorId: null, category: null, isNewest: false);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].CreatedDate == data2019);
            Assert.IsTrue(result[1].CreatedDate == data2020);
            Assert.IsTrue(result[2].CreatedDate == data2021);
        }

        [Test]
        public void GetNewsItem_RequestManyItem_ReturnAllItemsAndNewestFirst()
        {
            DateTime data2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime data2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime data2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data2021,
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2019,
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2020,
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll()).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = (List<NewsItem>)newsService.GetNewsItem(authorId: null, category: null);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].CreatedDate == data2021);
            Assert.IsTrue(result[1].CreatedDate == data2020);
            Assert.IsTrue(result[2].CreatedDate == data2019);
        }
    }
}