namespace person_wpf_demo.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Rue { get; set; }
        public string Ville { get; set; }
        public string CodePostal { get; set; }

        public int PersonId { get; set; }
        public Person Personne { get; set; }
    }
}
