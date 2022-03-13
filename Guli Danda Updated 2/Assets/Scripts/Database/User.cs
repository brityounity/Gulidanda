using System;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class User
{
        public string player_id { get; set; }
        public string player_firebase_id { get; set; }
        public string player_name { get; set; }
        public int total_coin { get; set; }
        public int current_danda { get; set; }
        public int current_guli { get; set; }
        public int total_match { get; set; }
        public int total_win { get; set; }
        public List<Danda> dandas { get; set; }
        public List<Guli> gulis { get; set; }
        public List<Environment> environments { get; set; }
}
[Serializable]
public class Danda{
     public int danda_id { get; set; } 
     public int buying_price {get; set;}
}
[Serializable]
public class Guli{
     public int guli_id { get; set; } 
     public int buying_price {get; set;}
}
[Serializable]
public class Environment{
     public int environment_id { get; set; } 
     public int unlock_cost {get; set;}
     public string environment_name {get; set;}
}