Credits to @ismailmayat and The Cogworks for the original Umbraco CogReview package

There are steps necessary to make the dashboard work

1) You will need for your documents to have a DateTime property, ideally with an alias of 'reviewDate' (although you can tweak that value)

2) You will need to tell the Umbraco Internal Index about this property and that it should be sortable, Locate your /config/examineindex.config file, and add the reviewDate as an IndexUserField to the InternalIndexSet eg:

<IndexSet SetName="InternalIndexSet" IndexPath="~/App_Data/TEMP/ExamineIndexes/{machinename}/Internal/">
    <IndexUserFields>
      <add Name="reviewDate" EnableSorting="true" />
    </IndexUserFields>
    
  </IndexSet>

3) Good, now copy the tooorangey.ContentReview folder and it's contents inside your site's App_Plugins Folder.

4) now copy the tooorangey.ContentReview.dll into the bin folder of your site.

5) Now to configure the dashboard to appear locate the /config/dashboards.config file in your umbraco site, and add the configuration to point to the dashboard plugin eg:

  <section alias="Content Review">
    <areas>
      <area>content</area>
    </areas>
    <tab caption="Content Review">
      <control>
        /app_plugins/tooorangey.ContentReview/ContentReviewDashboard.html
      </control>
    </tab>
  </section>


6) You should now be able to login to the Umbraco backoffice and see the Content Review dashboard, visit some pages of your site and set the 'Review Date' to be a date in the past, and then for another to be reviewed in the next 14 days, and another for some time  in the next 30 days - the dashboard should show all three items with the expired item marked as red, the soon to expire marked as Amber, and the other as Green, if the date is further forward than 30 days it won't appear on the dashboard.

7) to tweak the settings: have a look in the file in the dashboard folder called contentreview.controller.js this had a ‘dashboard’ configuration variable:

    vm.dashboard = {
        loading: false,
        noOfDaysAhead: 30,
        documentTypeWhiteList: '', // comma delimited string of doc types to include if blank includes all...
        indexSetSearcher: "InternalSearcher",
        reviewDateFieldAlias: "reviewDate"
    };

you can alter these values if you use a different alias for your review date or different index or want to set a greater number of days ahead.