﻿namespace Evenda.UI.Dtos
{
    public class PaginationDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string SortDir { get; set; }
    }
}
