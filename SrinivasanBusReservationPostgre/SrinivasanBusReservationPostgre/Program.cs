// See https://aka.ms/new-console-template for more information
using SrinivasanBusReservationPostgre;

Console.WriteLine("Hello, World!");

Connect con = new Connect();

home:
    do
    {
        Console.WriteLine("Enter 1.Admin\n2.User");
        int who = Convert.ToInt32(Console.ReadLine());
        switch (who)
        {
            case 1:
                do
                {
                    Console.WriteLine("Enter 1. Add new bus\n2. To view list\n3. To update bus\n4. To print passengers list\n5. For availability check\n9. To go back");
                    int adOpt = Convert.ToInt32(Console.ReadLine());
                    switch (adOpt)
                    {
                        case 1:
                            Console.WriteLine("Enter Bus Id");
                            int i = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter Bus Name");
                            String n = Console.ReadLine();
                            Console.WriteLine("Enter Bus Capacity");
                            int c = Convert.ToInt32(Console.ReadLine());
                            con.Insert(i, n, c);
                            break;
                        case 2:
                            con.DetailAll();
                            break;
                        case 3:
                            Console.WriteLine("Enter the Bus Id");
                            int b = Convert.ToInt32(Console.ReadLine());
                            con.UpdateOne(b);
                            break;
                        case 4:
                            con.ViewPassenger();
                            break;
                        case 5:
                            con.Availability();
                            break;
                        default:
                            goto home;
                    }
                } while (true);
            case 2:
                do
                {
                    Console.WriteLine("Enter 1. For availability check\n2. For booking tickets\n9. To go back");
                    int usrOpt = Convert.ToInt32(Console.ReadLine());
                    switch (usrOpt)
                    {
                        case 1:
                            con.Availability();
                            break;
                        case 2:
                            con.AddPassenger();
                            break;
                        default: goto home;
                    }
                } while (true);
            default: return;
        }
    } while (true);

//connect.Insert(101,"SRSDLX",10);
//connect.AddPassenger();
//connect.DetailAll();
//connect.UpdateOne(101);
//connect.Availability();
//connect.ViewPassenger();