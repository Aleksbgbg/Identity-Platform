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
        }

        public int CurrentPage { get; }

        public int ItemsPerPage { get; }

        public int TotalPages { get; }
    }
}