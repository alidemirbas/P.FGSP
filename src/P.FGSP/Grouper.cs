namespace P.FGSP
{
    public class Grouper
    {
        public string FieldName { get; }
        public Aggregate Aggregate { get; set; }
    }


    /*
        bundan pay bic
         SELECT
            t.AssessorParcelNumber,
            t.[r_number],
            t.Closed,
            SUM([assoc_balance]),                               <= Aggregate.Sum
            SUM([rr_balance]),                                  <= Aggregate.Sum
            SUM([_balance]),                                    <= Aggregate.Sum
            SUM([balance])                                      <= Aggregate.Sum
        FROM (table) t
        GROUP BY 
            t.AssessorParcelNumber,                             <= Aggregate.GroupBy
            t.r_number,                                         <= Aggregate.GroupBy
            t.Closed                                            <= Aggregate.GroupBy


        GroupBy'da Enum'lara katildi. ayri dusunmeye gerek yok bububu field'lar gruplanacak sususu field;lar ise sum/max/avg vs edilecek seklinde kullanirsin

        
        List<Grouper> olur
    */

    /*
        .GroupBy("new (ContractReference, CreaUserAccount.Username)", "it")

        .Select("new (Sum(ContractPosition) as ContractPosition, Count() as Contracts, ContractReference as ContractReference, CreaUserAccount.Username as CreaUserAccountUsername )")
     */


}
