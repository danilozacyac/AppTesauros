﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class TomoTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public String Descripcion { get; set; }
    }
}