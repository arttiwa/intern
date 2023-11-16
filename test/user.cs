using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.models{
public class GetUser {
    public int id { get; set;}
    public required string username { get; set;}
    public required int password { get; set; }
    public Boolean admin { get; set; }
    public required string email { get; set; }
    public required string name { get; set; }

}
}