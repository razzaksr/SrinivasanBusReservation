using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleTables;
using System.Security.Cryptography;
using System.Numerics;
using System.Collections;
using Npgsql;
using Npgsql.Internal;

namespace SrinivasanBusReservationPostgre
{
    internal class Avail
    {
        public int busId;
        public String name;
        public DateTime date;
        public int capacity;
        public Int64 availablity;
    }
    internal class Connect
    {
        NpgsqlConnection npgsqlConnection;
        NpgsqlCommand npgsqlCommand;
        NpgsqlDataReader npgsqlDataReader;

        public Connect()
        {
            
        }
        public void ViewPassenger()
        {
            var tab = new ConsoleTable("S.No", "Passenger Name", "Age");
            Console.WriteLine("Enter the bus id");
            int bid = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the date of journey");
            DateTime dt = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine(dt.Date);
            npgsqlConnection = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=booking");
            npgsqlCommand = new NpgsqlCommand("select * from passengers where bus_id=@i and doj=@d", npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@i", bid);
            npgsqlCommand.Parameters.AddWithValue("@d", dt.Date);
            npgsqlConnection.Open();
            npgsqlDataReader = npgsqlCommand.ExecuteReader();
            while (npgsqlDataReader.Read())
            {
                tab.AddRow(npgsqlDataReader["sno"], npgsqlDataReader["name"], npgsqlDataReader["age"]);
            }
            npgsqlConnection.Close();
            tab.Write();
        }
        public void Availability()
        {
            var tab=new ConsoleTable("Bus ID","Name","Date","Capacity","Available");
            Console.WriteLine("Enter the date of journey");
            DateTime dt = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine(dt.Date);
            npgsqlConnection = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=booking");
            npgsqlCommand = new NpgsqlCommand("select distinct doj,bus.id,bus.name,bus.capacity from passengers inner join bus on bus_id=bus.id where doj=@d", npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@d", dt.Date);
            npgsqlConnection.Open();
            npgsqlDataReader = npgsqlCommand.ExecuteReader();
            ArrayList ar = new ArrayList();
            while (npgsqlDataReader.Read())
            {
                Avail a = new Avail() 
                { 
                    busId =(int)npgsqlDataReader[1],
                    name=(String)npgsqlDataReader[2],
                    date=(DateTime)npgsqlDataReader[0],
                    capacity= (int)npgsqlDataReader[3],
                };
                ar.Add(a);
            }
            npgsqlConnection.Close();
            for(int index=0;index<ar.Count;index++)
            {
                Avail a = (Avail) ar[index];
                npgsqlCommand = new NpgsqlCommand("select count(*) from passengers where bus_id=@i and doj=@d", npgsqlConnection);
                npgsqlCommand.Parameters.AddWithValue("@i", a.busId);
                npgsqlCommand.Parameters.AddWithValue("@d", a.date);
                npgsqlConnection.Open();
                npgsqlDataReader = npgsqlCommand.ExecuteReader();
                if (npgsqlDataReader.Read())
                {
                    a.availablity = a.capacity - (Int64)npgsqlDataReader[0];
                    tab.AddRow(a.busId, a.name, a.date, a.capacity, a.availablity);
                }
            }
            tab.Write();
        }
        public void AddPassenger()
        {
            Console.WriteLine("Enter the bus id");
            int bid = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the date of journey");
            DateTime dt = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine(dt.Date);
            npgsqlConnection = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=booking");
            npgsqlCommand = new NpgsqlCommand("select count(*) from passengers where bus_id=@i and doj=@d", npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@i", bid);
            npgsqlCommand.Parameters.AddWithValue("@d", dt.Date);
            npgsqlConnection.Open();
            npgsqlDataReader = npgsqlCommand.ExecuteReader();
            if (npgsqlDataReader.Read())
            {
                Int64 asNow = (Int64)npgsqlDataReader[0];
                npgsqlConnection.Close();
                Console.WriteLine(asNow);
                npgsqlCommand = new NpgsqlCommand("select capacity from bus where id=@i", npgsqlConnection);
                npgsqlCommand.Parameters.AddWithValue("@i", bid);
                npgsqlConnection.Open();
                npgsqlDataReader = npgsqlCommand.ExecuteReader();
                if (npgsqlDataReader.Read())
                {
                    int asFact = (int)npgsqlDataReader["capacity"];
                    npgsqlConnection.Close();
                    if (asFact > asNow)
                    {
                        npgsqlConnection.Close();
                        Console.WriteLine("Enter passenger name and age");
                        String pname = Console.ReadLine();
                        int age = Convert.ToInt32(Console.ReadLine());
                        npgsqlCommand = new NpgsqlCommand("insert into passengers(name,age,doj,bus_id) values(@n,@a,@d,@b)", npgsqlConnection);
                        npgsqlCommand.Parameters.AddWithValue("@n", pname);
                        npgsqlCommand.Parameters.AddWithValue("@a", age);
                        npgsqlCommand.Parameters.AddWithValue("@d", dt);
                        npgsqlCommand.Parameters.AddWithValue("@b", bid);
                        npgsqlConnection.Open();
                        npgsqlCommand.ExecuteNonQuery();
                        Console.WriteLine("Booking created successfully");
                        npgsqlConnection.Close();
                    }
                    else
                    {
                        Console.WriteLine("Seats are full");
                    }
                }
            }
        }
        public void UpdateOne(int bid)
        {
            npgsqlConnection = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=booking");
            npgsqlCommand=new NpgsqlCommand("select * from bus where id=@i", npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@i", bid);
            npgsqlConnection.Open();
            npgsqlDataReader = npgsqlCommand.ExecuteReader();
            if (npgsqlDataReader.Read())
            {
                bid= (int)npgsqlDataReader["id"];
                String nm =(String)npgsqlDataReader["name"];
                int cap = (int)npgsqlDataReader["capacity"];
                Console.WriteLine("Found " + bid + " and " + nm + " with " + cap);
                npgsqlConnection.Close();
                Console.WriteLine("Enter the new name to " + bid);
                nm=Console.ReadLine();
                Console.WriteLine("Enter the new Capacity to " + bid);
                cap=Convert.ToInt32(Console.ReadLine());
                npgsqlCommand = new NpgsqlCommand("update bus set name=@n, capacity=@c where id=@i", npgsqlConnection);
                npgsqlCommand.Parameters.AddWithValue("@i", bid);
                npgsqlCommand.Parameters.AddWithValue("@n", nm);
                npgsqlCommand.Parameters.AddWithValue("@c", cap);
                npgsqlConnection.Open();
                npgsqlCommand.ExecuteNonQuery();
                Console.WriteLine("Record has updated");
                npgsqlConnection.Close();
            }
            else
            {
                Console.WriteLine("Bus ID not found");
            }
        }
        public void DetailAll()
        {
            npgsqlConnection = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=booking");
            npgsqlCommand = new NpgsqlCommand(@"select * from bus", npgsqlConnection);
            npgsqlConnection.Open();
            npgsqlDataReader = npgsqlCommand.ExecuteReader();
            var tab = new ConsoleTable("Bus ID", "Bus Name", "Bus Capacity");
                while (npgsqlDataReader.Read())
                {
                //Console.WriteLine(_reader["id"] + " " + _reader["name"] + " " + _reader["capacity"]);
                tab.AddRow(npgsqlDataReader["id"], npgsqlDataReader["name"], npgsqlDataReader["capacity"]);
                }
            tab.Write();
            npgsqlConnection.Close();
        }

        public void Insert(int bid,String bname,int bcap)
        {
            npgsqlConnection = new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=booking");
            npgsqlCommand = new NpgsqlCommand(@"insert into bus(id,name,capacity) values(@i,@n,@c)",npgsqlConnection);
            npgsqlCommand.Parameters.AddWithValue("@i", bid);
            npgsqlCommand.Parameters.AddWithValue("@n", bname);
            npgsqlCommand.Parameters.AddWithValue("@c", bcap);
            npgsqlConnection.Open();
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();
            Console.WriteLine("Record has inserted");
            
        }

        ~Connect()
        {
            npgsqlConnection.Close();
        }
    }
}
