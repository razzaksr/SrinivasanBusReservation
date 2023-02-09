using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using ConsoleTables;
using System.Security.Cryptography;
using System.Numerics;
using System.Collections;

namespace SrinivasanBusReservation
{
    internal class Avail
    {
        public int busId;
        public String name;
        public DateTime date;
        public int capacity;
        public int availablity;
    }
    internal class Connect
    {
        SqlConnection _connection;
        SqlCommand _command;
        SqlDataReader _reader;
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
            _connection = new SqlConnection("Data Source=SRDB\\SQLEXPRESS;Initial Catalog=booking;Integrated Security=True");
            _command = new SqlCommand("select * from passengers where bus_id=@i and doj=@d", _connection);
            _command.Parameters.AddWithValue("@i", bid);
            _command.Parameters.AddWithValue("@d", dt.Date);
            _connection.Open();
            _reader = _command.ExecuteReader();
            while (_reader.Read())
            {
                tab.AddRow(_reader["sno"], _reader["name"], _reader["age"]);
            }
            _connection.Close();
            tab.Write();
        }
        public void Availability()
        {
            var tab=new ConsoleTable("Bus ID","Name","Date","Capacity","Available");
            Console.WriteLine("Enter the date of journey");
            DateTime dt = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine(dt.Date);
            _connection = new SqlConnection("Data Source=SRDB\\SQLEXPRESS;Initial Catalog=booking;Integrated Security=True");
            _command = new SqlCommand("select distinct doj,bus.id,bus.name,bus.capacity from passengers inner join bus on bus_id=bus.id where doj=@d", _connection);
            _command.Parameters.AddWithValue("@d", dt.Date);
            _connection.Open();
            _reader = _command.ExecuteReader();
            ArrayList ar = new ArrayList();
            while (_reader.Read())
            {
                Avail a = new Avail() 
                { 
                    busId =(int)_reader[1],
                    name=(String)_reader[2],
                    date=(DateTime)_reader[0],
                    capacity=(int)_reader[3],
                };
                ar.Add(a);
            }
            _connection.Close();
            for(int index=0;index<ar.Count;index++)
            {
                Avail a = (Avail) ar[index];
                _command = new SqlCommand("select count(*) from passengers where bus_id=@i and doj=@d", _connection);
                _command.Parameters.AddWithValue("@i", a.busId);
                _command.Parameters.AddWithValue("@d", a.date);
                _connection.Open();
                _reader = _command.ExecuteReader();
                if (_reader.Read())
                {
                    a.availablity = a.capacity - (int)_reader[0];
                    tab.AddRow(a.busId, a.name, a.date, a.capacity, a.availablity);
                    //int asNow = (int)_reader[0];
                    //tab.AddRow(rows[0, 0], _reader[2], _reader[0], _reader[3], (int)_reader[3] - asNow);
                }
            }
            tab.Write();
            /*
            _command = new SqlCommand("select distinct doj,bus.id,bus.name,bus.capacity from passengers inner join bus on bus_id=bus.id where doj=@d", _connection);
            //_command = new SqlCommand("select doj,bus.id,bus.name,bus.capacity from passengers inner join bus on bus_id=bus.id where doj=@d", _connection);
            //_command = new SqlCommand("select passengers.doj,id,name,capacity from bus inner join passengers on bus.id=passengers.bus_id where doj=@d", _connection);
            _command.Parameters.AddWithValue("@d", dt.Date);
            _connection.Open();
            _reader = _command.ExecuteReader();
            if (_reader.Read())
            {

            }
            _connection.Close();
            */
            /*
            _command = new SqlCommand("select count(*) from passengers where bus_id=@i and doj=@d", _connection);
            _command.Parameters.AddWithValue("@i", 101);
            _command.Parameters.AddWithValue("@d", DateTime.Parse("20/02/2023"));
            _connection.Open();
            _reader = _command.ExecuteReader();
            if (_reader.Read())
            {
                int asNow = (int)_reader[0];
                //tab.AddRow(rows[0, 0], _reader[2], _reader[0], _reader[3], (int)_reader[3] - asNow);
            }


            tab.Write();
            */
        }
        public void AddPassenger()
        {
            Console.WriteLine("Enter the bus id");
            int bid = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the date of journey");
            DateTime dt = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine(dt.Date);
            _connection = new SqlConnection("Data Source=SRDB\\SQLEXPRESS;Initial Catalog=booking;Integrated Security=True");
            _command = new SqlCommand("select count(*) from passengers where bus_id=@i and doj=@d", _connection);
            _command.Parameters.AddWithValue("@i", bid);
            _command.Parameters.AddWithValue("@d", dt.Date);
            _connection.Open();
            _reader = _command.ExecuteReader();
            if (_reader.Read())
            {
                int asNow = (int)_reader[0];
                _connection.Close();
                Console.WriteLine(asNow);
                _command = new SqlCommand("select capacity from bus where id=@i", _connection);
                _command.Parameters.AddWithValue("@i", bid);
                _connection.Open();
                _reader = _command.ExecuteReader();
                if (_reader.Read())
                {
                    int asFact = (int)_reader["capacity"];
                    _connection.Close();
                    if (asFact > asNow)
                    {
                        _connection.Close();
                        Console.WriteLine("Enter passenger name and age");
                        String pname = Console.ReadLine();
                        int age = Convert.ToInt32(Console.ReadLine());
                        _command = new SqlCommand("insert into passengers(name,age,doj,bus_id) values(@n,@a,@d,@b)", _connection);
                        _command.Parameters.AddWithValue("@n", pname);
                        _command.Parameters.AddWithValue("@a", age);
                        _command.Parameters.AddWithValue("@d", dt);
                        _command.Parameters.AddWithValue("@b", bid);
                        _connection.Open();
                        _command.ExecuteNonQuery();
                        Console.WriteLine("Booking created successfully");
                        _connection.Close();
                    }
                    else
                    {
                        Console.WriteLine("Seats are full");
                    }
                }
            }
            /*
            
            
            
            /*
            if (_reader.Read())
            {
                Console.WriteLine("Journey available " + _reader[0]);
                if ((int)_reader[0] > 0)
                {
                    Console.WriteLine(_reader["doj"]);
                }
            }
            else
            {
                Console.WriteLine("Journey not available start it");
            }*/

        }
        public void UpdateOne(int bid)
        {
            _connection = new SqlConnection("Data Source=SRDB\\SQLEXPRESS;Initial Catalog=booking;Integrated Security=True");
            _command = new SqlCommand("select * from bus where id=@i", _connection);
            _command.Parameters.AddWithValue("@i", bid);
            _connection.Open();
            _reader = _command.ExecuteReader();
            if (_reader.Read())
            {
                bid= (int)_reader["id"];
                String nm =(String) _reader["name"];
                int cap = (int)_reader["capacity"];
                Console.WriteLine("Found " + bid + " and " + nm + " with " + cap);
                _connection.Close();
                Console.WriteLine("Enter the new name to " + bid);
                nm=Console.ReadLine();
                Console.WriteLine("Enter the new Capacity to " + bid);
                cap=Convert.ToInt32(Console.ReadLine());
                _command = new SqlCommand("update bus set name=@n, capacity=@c where id=@i", _connection);
                _command.Parameters.AddWithValue("@i", bid);
                _command.Parameters.AddWithValue("@n", nm);
                _command.Parameters.AddWithValue("@c", cap);
                _connection.Open();
                _command.ExecuteNonQuery();
                Console.WriteLine("Record has updated");
                _connection.Close();
            }
            else
            {
                Console.WriteLine("Bus ID not found");
            }
        }
        public void DetailAll()
        {
            _connection = new SqlConnection("Data Source=SRDB\\SQLEXPRESS;Initial Catalog=booking;Integrated Security=True");
                _command = new SqlCommand("select * from bus", _connection);
                _connection.Open();
                _reader = _command.ExecuteReader();
            var tab = new ConsoleTable("Bus ID", "Bus Name", "Bus Capacity");
                while (_reader.Read())
                {
                //Console.WriteLine(_reader["id"] + " " + _reader["name"] + " " + _reader["capacity"]);
                tab.AddRow(_reader["id"], _reader["name"], _reader["capacity"]);
                }
            tab.Write();
            _connection.Close();
        }

        public void Insert(int bid,String bname,int bcap)
        {
            _connection = new SqlConnection("Data Source=SRDB\\SQLEXPRESS;Initial Catalog=booking;Integrated Security=True");
            _command = new SqlCommand("insert into bus(id,name,capacity) values(@i,@n,@c)",_connection);
            _command.Parameters.AddWithValue("@i", bid);
            _command.Parameters.AddWithValue("@n", bname);
            _command.Parameters.AddWithValue("@c", bcap);
            _connection.Open();
            _command.ExecuteNonQuery();
            _connection.Close();
            Console.WriteLine("Record has inserted");
            
        }

        ~Connect()
        {
            _connection.Close();
        }
    }
}
