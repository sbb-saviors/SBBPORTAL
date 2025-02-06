using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Helpers
{
    public static class DataTableUtils
    {

        public static (List<T> Data, int RecordsTotal, int RecordsFiltered) ApplyDataTableParameters<T>(
            IQueryable<T> model,
            HttpRequest request,
            Dictionary<string, Expression<Func<T, string>>> searchFields,
            string defaultSortColumn = "Id")
        {

            int pageSize = 300;
            int skip = 0;

            try
            {
                var draw = request.Form != null ? request.Form["draw"].FirstOrDefault() ?? null : null;
                var start = request.Form != null ? request.Form["start"].FirstOrDefault() ?? null : null;
                var length = request.Form != null ? request.Form["length"].FirstOrDefault() ?? null : null;
                var sortColumn = request.Form != null
                    ? request.Form["columns[" + (request.Form["order[0][column]"].FirstOrDefault() ?? "") + "][data]"].FirstOrDefault() ?? null
                    : null;
                var sortColumnDirection = request.Form != null ? request.Form["order[0][dir]"].FirstOrDefault() ?? null : null;
                var searchValue = request.Form != null ? request.Form["search[value]"].FirstOrDefault() ?? null : null;

                pageSize = length != null ? Convert.ToInt32(length) : 400;
                skip = start != null ? Convert.ToInt32(start) : 0;

                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    model = model.OrderByFromString(sortColumn, sortColumnDirection == "asc");
                }
                else
                {
                    model = model.OrderByDescending(o => EF.Property<object>(o, defaultSortColumn));
                }

                // Filtreleme
                if (!string.IsNullOrEmpty(searchValue))
                {
                    var predicate = PredicateBuilder.New<T>(false); // Başlangıçta false olan bir predikat oluşturun

                    foreach (var field in searchFields)
                    {
                        var fieldExpression = field.Value; // İfadenin kendisi
                        predicate = predicate.Or(item => fieldExpression.Invoke(item).Contains(searchValue.ToLower())); // Predikata yeni koşul ekle
                    }

                    model = model.AsExpandable().Where(predicate); // Predikatı uygula
                }
            }
            catch (Exception)
            {
                
            }

            int recordsTotal = model.Count();
            int recordsFiltered = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();

            return (data, recordsTotal, recordsFiltered);
        }
    }
}
