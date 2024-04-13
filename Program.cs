using AnimalRestAPI.entity;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Animal MapToAnimal(MySqlDataReader reader)
{
    return new Animal
    {
        Id = reader.GetInt32(0),
        Name = reader.GetString(1),
        Category = (Category)reader.GetInt32(2),
        Breed = reader.GetString(3),
        Color = reader.GetString(4)
    };
}

Visit MapToVisit(MySqlDataReader reader)
{
    return new Visit
    {
        Id = reader.GetInt32(0),
        AnimalId = reader.GetInt32(1),
        VisitDate = reader.GetDateTime(2),
        Description = reader.GetString(3),
        Price = reader.GetInt32(4)
    };
}

app.MapGet("animal/all", (IConfiguration configuration) =>
{
    var animals = new List<Animal>();
    using (var sqlConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
    {
        var sqlCommand = new MySqlCommand("SELECT * FROM Animals", sqlConnection);
        sqlCommand.Connection?.Open();
        var reader = sqlCommand.ExecuteReader();
        while (reader.Read())
        {
            animals.Add(MapToAnimal(reader));
        }
    }
    return animals.Count == 0 ? Results.NotFound("Brak zwierząt w bazie") : Results.Ok(animals);
});

app.MapGet("animal/{id:int}", (IConfiguration configuration, int id) =>
{
    using var sqlConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    var sqlCommand = new MySqlCommand("SELECT * FROM Animals WHERE Id = @id", sqlConnection);
    sqlCommand.Parameters.AddWithValue("@id", id);
    sqlCommand.Connection?.Open();
    var reader = sqlCommand.ExecuteReader();
    if (!reader.Read()) return Results.NotFound("Brak zwierzaka o id: " + id);
    return Results.Ok(MapToAnimal(reader));
});


app.MapPost("animal/add", async (IConfiguration configuration, Animal animal) =>
{
    await using var sqlConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    var sqlCommand = new MySqlCommand("INSERT INTO Animals (Name, Category, Breed, Color) VALUES (@name, @category, @breed, @color); SELECT LAST_INSERT_ID();", sqlConnection);
    sqlCommand.Parameters.AddWithValue("@name", animal.Name);
    sqlCommand.Parameters.AddWithValue("@category", (int)animal.Category);
    sqlCommand.Parameters.AddWithValue("@breed", animal.Breed);
    sqlCommand.Parameters.AddWithValue("@color", animal.Color);
    sqlCommand.Connection?.Open();
    var newId = await sqlCommand.ExecuteScalarAsync();
    animal.Id = Convert.ToInt32(newId);
    return Results.Created($"/animal/{animal.Id}", animal);
});

app.MapPut("animal/{id:int}", async (IConfiguration configuration, int id, Animal animal) =>
{
    await using var sqlConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    var sqlCommand = new MySqlCommand("UPDATE Animals SET Name = @name, Category = @category, Breed = @breed, Color = @color WHERE Id = @id", sqlConnection);
    sqlCommand.Parameters.AddWithValue("@name", animal.Name);
    sqlCommand.Parameters.AddWithValue("@category", (int)animal.Category);
    sqlCommand.Parameters.AddWithValue("@breed", animal.Breed);
    sqlCommand.Parameters.AddWithValue("@color", animal.Color);
    sqlCommand.Parameters.AddWithValue("@id", id);
    sqlCommand.Connection?.Open();
    var rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
    return rowsAffected == 0 ? Results.NotFound("Brak zwierzaka o id: " + id) : Results.Ok();
});

app.MapDelete("animal/{id:int}", async (IConfiguration configuration, int id) =>
{
    await using var sqlConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    var sqlCommand = new MySqlCommand("DELETE FROM Animals WHERE Id = @id", sqlConnection);
    sqlCommand.Parameters.AddWithValue("@id", id);
    sqlCommand.Connection?.Open();
    var rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
    return rowsAffected == 0 ? Results.NotFound("Brak zwierzaka o id: " + id) : Results.Ok();
});

app.MapPost("animal/{animalId:int}/visit/add", async (IConfiguration configuration, int animalId, Visit visit) =>
{
    using var sqlConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
    var sqlCommand = new MySqlCommand("INSERT INTO Visits (AnimalId, VisitDate, Description, Price) VALUES (@animalId, @visitDate, @description, @price); SELECT LAST_INSERT_ID();", sqlConnection);
    sqlCommand.Parameters.AddWithValue("@animalId", animalId);
    sqlCommand.Parameters.AddWithValue("@visitDate", visit.VisitDate);
    sqlCommand.Parameters.AddWithValue("@description", visit.Description);
    sqlCommand.Parameters.AddWithValue("@price", visit.Price);
    sqlCommand.Connection?.Open();
    var newId = await sqlCommand.ExecuteScalarAsync();
    visit.Id = Convert.ToInt32(newId);
    return Results.Created($"/animal/{animalId}/visit/{visit.Id}", visit);
});

app.MapGet("animal/{animalId:int}/visits", async (IConfiguration configuration, int animalId) =>
{
    var visits = new List<Visit>();
    await using (var sqlConnection = new MySqlConnection(configuration.GetConnectionString("DefaultConnection")))
    {
        var sqlCommand = new MySqlCommand("SELECT * FROM Visits WHERE AnimalId = @animalId", sqlConnection);
        sqlCommand.Parameters.AddWithValue("@animalId", animalId);
        
        await sqlConnection.OpenAsync();
        var reader = await sqlCommand.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            visits.Add(MapToVisit(reader));
        }
    }
    return visits.Count == 0 ? Results.NotFound("Brak wizyt dla zwierzęcia o id: " + animalId) : Results.Ok(visits);
});

app.Run();