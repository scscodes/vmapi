# Vending Machine API v2
CRUD resources associated with a sample vending machine.  

## Requirements

### Dispense Item
- Where: TransactionsController/PostTransaction( )
- What: Validate and Set quantity, total cost, and change amount
- Detail: Uses private methods to validate items, inventories and costs. 
Additional private methods exist to mock/facilitate what would be a shared method that enables TransactionsController to interact with the "Machine" as if it were going to utilize a third party integration, or a physical interface.

### Get Item
- Where: ItemsController/GetItem( id )
- What: Return individual item by Id unique key


### Sample Payload & Output
- PostTransaction( [valid payload] )
```
// SAMPLE PAYLOAD
{
  "id": 9,
  "transactionLineItem": [
    {
      "id": 15,
      "transactionId": 9,
      "machineId": 20,
      "itemId": 1,
      "quantity": 7
    },
{
      "id": 16,
      "transactionId": 9,
      "machineId": 20,
      "itemId": 3,
      "quantity": 6
    }
  ],
  "amountTendered": 95
}
```

```
// CONSOLE OUTPUT 
## Dispensing the following items:
[{"Id":15,"TransactionId":9,"MachineId":20,"ItemId":1,"Quantity":7},{"Id":16,"TransactionId":9,"MachineId":20,"ItemId":3,"Quantity":6}]
## Total Change: 21.13
{"Hundreds":0,"Fifties":0,"Twenties":1,"Tens":0,"Fives":0,"Ones":1,"Quarters":0,"Dimes":1,"Nickles":0,"Pennies":3}

```

- GetItem( id )
```
// PAYLOAD RESPONSE
{
  "id": 1,
  "name": "Light",
  "description": "Rail mounted light",
  "price": 1.99
}
```

## Resources
### NuGet Packages
- EntityFrameworkCore v6.0.8
- EntityFrameworkCore.Tools v6.0.8
- EntityFrameworkCore.Sqlite v6.0.8
- EntityFrameworkCore.Sqlite.Core v6.0.8
- VisualStudio.Web.CodeGeneration.Design v6.0.8  
~~- VisualStudio.Azure.Containers.Tools.Targets v1.15.1~~
- Swashbuckle.AspNetCore v6.2.3

### Third Party Software
- SQLite ~v3
- (optional) DB Browser ([SQLite](https://sqlitebrowser.org/))

### Data Source
- Generate DB Schema and Initialize Database:
```
dotnet ef migrations initial
dotnet ef database update
```
- Insert Mock Data. From a SQL Editor/Query Window:
```
/Data/mockdata.sql
```
