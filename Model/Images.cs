//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Audi.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Images
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
    
        public virtual Products Products { get; set; }
    }
}
