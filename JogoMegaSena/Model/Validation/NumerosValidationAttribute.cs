using System.ComponentModel.DataAnnotations;

namespace JogoMegaSena.Model.Validation
{
    public class NumerosValidationAttribute : ValidationAttribute
    {
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    if(ValidarNumerosDiferentes())
        //        return ValidationResult.Success;
        //    return new ValidationResult("Não pode conter núemros repetidos!");

        //}

        //private bool ValidarNumerosDiferentes()
        //{
        //    // Use um HashSet para rastrear números únicos.
        //    HashSet<int> numerosUnicos = new HashSet<int>();

        //    foreach (int numero in numeros)
        //    {
        //        // Se tentarmos adicionar um número que já existe no HashSet, não será adicionado novamente.
        //        if (!numerosUnicos.Add(numero))
        //        {
        //            return false; // Encontramos um número repetido, a validação falha.
        //        }
        //    }

        //    // Se chegarmos até aqui, todos os números são diferentes.
        //    return true;

        //}
    }
}