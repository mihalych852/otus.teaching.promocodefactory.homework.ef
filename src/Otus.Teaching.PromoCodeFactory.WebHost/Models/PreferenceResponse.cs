using System;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    /// <summary>
    /// Предпочтение
    /// </summary>
    public class PreferenceResponse
    {
        /// <summary>
        /// Идентификатор предпочтения
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Название предпочтения
        /// </summary>
        public string Name { get; set; }
    }
}