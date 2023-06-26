using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {





        string connectionString = "Server=34.218.6.36,1435\\evaluaciones;Database=evaluacion_cjimenez;User Id=sa;Password=abcd1234";



        [HttpPost]
        public IActionResult InsertarTicket([FromBody] Ticket ticket)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {



                    SqlCommand command = new SqlCommand("InsertarTicket", connection);
                    command.CommandType = CommandType.StoredProcedure;



                    //string query = "INSERT INTO Tickets (Id_Tienda, Id_Registradora, FechaHora, Ticket, Impuesto, Total, FechaHora_Creacion) " +
                    //               "VALUES (@Id_Tienda, @Id_Registradora, @FechaHora, @Ticket, @Impuesto, @Total, @FechaHora_Creacion)";

                    //SqlCommand command = new SqlCommand(query, connection);


                    command.Parameters.AddWithValue("@Id_Tienda", ticket.Id_Tienda);
                    command.Parameters.AddWithValue("@Id_Registradora", ticket.Id_Registradora);
                    command.Parameters.AddWithValue("@FechaHora", ticket.FechaHora);
                    command.Parameters.AddWithValue("@Ticket", ticket.TicketId);
                    command.Parameters.AddWithValue("@Impuesto", ticket.Impuesto);
                    command.Parameters.AddWithValue("@Total", ticket.Total);
                    command.Parameters.AddWithValue("@FechaHora_Creacion", ticket.FechaHora_Creacion);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();



                    return Ok("Ticket insertado correctamente.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar el ticket: {ex.Message}");
            }
        }



        //            int rowsAffected = command.ExecuteNonQuery();
        //            connection.Close();

        //            if (rowsAffected > 0)
        //            {
        //                return Ok("Ticket insertado correctamente.");
        //            }
        //            else
        //            {
        //                return BadRequest("No se pudo insertar el ticket.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error al insertar el ticket: {ex.Message}");
        //    }
        //}




        [HttpGet]

        public IEnumerable <Ticket >Obtenertickets ()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                List<Ticket> tick = new List <Ticket>();


                connection.Open();


                string querya = "SELECT Id_Tienda, Id_Registradora, FechaHora, Ticket, Impuesto,Total,FechaHora_Creacion FROM tickets";

                using (SqlCommand command = new SqlCommand(querya, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Ticket oper = new Ticket
                            {
                                Id_Tienda = (String)reader["Id_Tienda"],
                                Id_Registradora = (String)reader["Id_Registradora"],
                                FechaHora = (DateTime)reader["FechaHora"],
                                TicketId = (int)reader["Ticket"],
                                Impuesto = (decimal)reader["Impuesto"],
                                Total = (decimal)reader ["Total"],
                                FechaHora_Creacion = (DateTime)reader["FechaHora_Creacion"]


                            };

                            tick.Add(oper);


                        }
                    }
                }


                return tick;
            }


        }




        [HttpPut("{ticketId}")]
        public IActionResult ActualizarTicket (int ticketId, [FromBody] Ticket ticket)
        {
            try
            {


                using (SqlConnection connection = new SqlConnection(connectionString))



                {
                    string query = "UPDATE Tickets SET Id_Tienda = @Id_Tienda, Id_Registradora = @Id_Registradora, " +
                                   "FechaHora = @FechaHora, Impuesto = @Impuesto, Total = @Total " +
                                   "WHERE Ticket = @TicketId";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Id_Tienda", ticket.Id_Tienda);
                    command.Parameters.AddWithValue("@Id_Registradora", ticket.Id_Registradora);
                    command.Parameters.AddWithValue("@FechaHora", ticket.FechaHora);
                    command.Parameters.AddWithValue("@Impuesto", ticket.Impuesto);
                    command.Parameters.AddWithValue("@Total", ticket.Total);
                    command.Parameters.AddWithValue("@TicketId", ticketId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        return Ok("Ticket actualizado correctamente.");
                    }
                    else
                    {
                        return NotFound("No se encontró el ticket especificado.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el ticket: {ex.Message}");
            }
        }

        [HttpDelete("{ticketId}")]
        public IActionResult EliminarTicket(int ticketId)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))

                {
                    string query = "DELETE FROM Tickets WHERE Ticket = @TicketId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TicketId", ticketId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        return Ok("Ticket eliminado correctamente.");
                    }
                    else
                    {
                        return NotFound("No se encontró el ticket especificado.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el ticket: {ex.Message}");
            }
        }







    }
}

