namespace Identity.Platform.Models.ViewModels
{
    using System;

    public class PagingInfo
    {
        public PagingInfo(int currentPage, int totalItems, int itemsPerPage)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            StartPage = currentPage - 2;
            EndPage = currentPage + 2;

            if (StartPage < 1)
            {
                EndPage -= StartPage;
                StartPage = 1;

                if (EndPage > TotalPages)
                {
                    EndPage = TotalPages;
                }
            }
            else if (EndPage > TotalPages)
            {
                StartPage -= EndPage - TotalPages;
                EndPage = TotalPages;
            }
        }

        public int CurrentPage { get; }

        public int ItemsPerPage { get; }

        public int TotalPages { get; }

        public int StartPage { get; }

        public int EndPage { get; }
    }
}