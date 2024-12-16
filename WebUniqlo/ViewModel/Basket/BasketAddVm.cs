namespace WebUniqlo.ViewModel.Basket
{
    public class BasketAddVm
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public BasketAddVm(int id)
        {
            Id=id;
            Count = 1;
        }

    }
}
