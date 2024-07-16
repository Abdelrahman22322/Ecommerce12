using System.ComponentModel.DataAnnotations;
using Castle.Components.DictionaryAdapter;

namespace Ecommerce.Core.Domain.Entities;

public class CloudinaryResult
{
    [System.ComponentModel.DataAnnotations.Key]
    public int Id { get; set; }
    public string Url { get; set; }
    public string PublicId { get; set; }
}