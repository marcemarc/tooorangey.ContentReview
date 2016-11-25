using Examine;
using Examine.LuceneEngine.Providers;
using Examine.Providers;
using Examine.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tooorangey.ContentReview.Services
{
   public class ContentReviewSearchService
    {
        private string _reviewDateFieldAlias { get; set; }
        private string _indexSearcher { get; set; }
        private string _docTypeWhitelist { get; set; }
        public ContentReviewSearchService(string reviewDateFieldAlias = "reviewDate", string indexSearcher = "InternalSearcher", string docTypeWhitelist = "")
        {
            _reviewDateFieldAlias = reviewDateFieldAlias;
            _indexSearcher = indexSearcher;
            _docTypeWhitelist = docTypeWhitelist;
        }
        public ISearchResults DocumentsByReviewDate(int noOfDaysAhead)
        {
            BaseSearchProvider searcher;
            string fieldToOrderBy = String.IsNullOrEmpty(_reviewDateFieldAlias) ? "updateDate" : _reviewDateFieldAlias;
           // string docTypeAliasWhiteList = "blogpost,blogpostrepository,landingpage,search,home,textpage";
            searcher = ExamineManager.Instance.SearchProviderCollection[_indexSearcher] as LuceneSearcher;
            var query = searcher.CreateSearchCriteria(BooleanOperation.And).Field("__IndexType", "content").And().Range(fieldToOrderBy, new DateTime(2000, 1, 1), DateTime.Now.AddDays(noOfDaysAhead));
            if (!String.IsNullOrEmpty(_docTypeWhitelist))
            {
                query = query.And().GroupedOr(new string[] { "__NodeTypeAlias" }, _docTypeWhitelist.Split(','));
            }
            query = query.And().OrderBy(fieldToOrderBy);

            var results = searcher.Search(query.Compile());

            return results;


        }

    }
}
