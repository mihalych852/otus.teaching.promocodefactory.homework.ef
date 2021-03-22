namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class MyDbInitializer : IMyDbInitializer
    {
        private readonly Context _context;

        public MyDbInitializer(Context context)
        {
            _context = context;
        }
        
        public void Init()
        {
            //_context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated())
            {
                _context.AddRange(FakeDataFactory.Employees);
                _context.SaveChanges();

                _context.AddRange(FakeDataFactory.Preferences);
                _context.SaveChanges();

                _context.AddRange(FakeDataFactory.Customers);
                _context.SaveChanges();

                _context.AddRange(FakeDataFactory.PromoCodes);
                _context.SaveChanges();
            }
        }
    }
}