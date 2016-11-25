using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using tooorangey.ContentReview.Services;
using Umbraco.Web.WebApi;

namespace tooorangey.ContentReview.Controllers
{
    public class ContentReviewListController : UmbracoAuthorizedApiController
    {
        [HttpGet]
       public ContentReviewSet ListReviewDocuments(int noOfDaysAhead = 30, int page = 1, int pageSize = 50,string documentTypeWhiteList = "", string indexSetSearcher = "InternalSearcher", string reviewDateFieldAlias = "reviewDate")
        {
            var contentReviewSearchService = new ContentReviewSearchService(reviewDateFieldAlias,indexSetSearcher,documentTypeWhiteList);
            var results = contentReviewSearchService.DocumentsByReviewDate(noOfDaysAhead);

            List<ReviewResult> reviewResults = new List<ReviewResult>();
            var skip = (page - 1) * pageSize;

            var pageCount = (int)Math.Ceiling((double)results.TotalItemCount / pageSize);


            foreach (var result in results.Skip(skip).Take(pageSize).Where(f=>f!=null))
            {
                var reviewResult = new ReviewResult();
                reviewResult.Id = result.Id.ToString();
                reviewResult.Name = result.Fields["nodeName"];
                reviewResult.ReviewDate = DateTime.Parse(result.Fields[reviewDateFieldAlias]);
                var status = ContentStatus.Green;
                var daysDifference = (reviewResult.ReviewDate - DateTime.Now).Days;
                reviewResult.DaysUntilReview = daysDifference;
                if (daysDifference < 14)
                {
                    status = ContentStatus.Amber;
                }
                if (daysDifference < 1)
                {
                    status = ContentStatus.Red;
                }
                reviewResult.Status = status;
                reviewResults.Add(reviewResult);
            }

            var resultSet = new ContentReviewSet()
            {
                Page = page,
                TotalItemCount = results.TotalItemCount,
                PageSize = pageSize,
                Results = reviewResults,
                NoOfDaysAhead = noOfDaysAhead,
                PageCount = pageCount

            };

            return resultSet;
        }


    }

    public class ContentReviewSet
    {
        public IEnumerable<ReviewResult> Results { get; set; }
        public int Page { get; set; }
        public int TotalItemCount { get; set; }
        public bool HasResults { get { return this.Results.Any(); } }
        public string StatusMessage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int NoOfDaysAhead { get; set; }
    }

    public class ReviewResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ReviewDate { get; set; }
        public ContentStatus Status { get; set; }
        public int DaysUntilReview { get; set; }
    }
    public enum ContentStatus
    {
        Red=1,
        Amber=2,
        Green=3
    }
}
