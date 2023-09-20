namespace JogoMegaSena.Configuration
{
    public class RetornoCustomizado<T>
    {
        public bool Sucesso { get; set; }
        public T Dados { get; set; }
        public string Menssagem { get; set; }
        public int Status { get; set; }
    }
}
