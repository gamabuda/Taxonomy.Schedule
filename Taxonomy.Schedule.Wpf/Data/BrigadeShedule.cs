//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Taxonomy.Schedule.Wpf.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class BrigadeShedule
    {
        public int Id { get; set; }
        public Nullable<int> Id_Brigade { get; set; }
        public Nullable<int> Id_Shedule { get; set; }
        public Nullable<int> Id_TypeChange { get; set; }
    
        public virtual Brigade Brigade { get; set; }
        public virtual Shedule Shedule { get; set; }
        public virtual TypeChange TypeChange { get; set; }
    }
}