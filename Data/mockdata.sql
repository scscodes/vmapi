/* Inject into DB of choice to build mock data set */

/* Build items */
INSERT INTO Item(Id, Name, Description, Price) VALUES (1, "Light", "Rail mounted light", 1.99);
INSERT INTO Item(Id, Name, Description, Price) VALUES (3, "Bucket", "Good for holding things. Not a helmet", 9.99);
INSERT INTO Item(Id, Name, Description, Price) VALUES (40, "Wheelbarrow", "Disappointing dump truck", 29.99);
INSERT INTO Item(Id, Name, Description, Price) VALUES (7, "Wrench", "Is not the size you need", 5.99);
INSERT INTO Item(Id, Name, Description, Price) VALUES (16, "Wire", "Solid copper core, 50 feet", 150.00);
INSERT INTO Item(Id, Name, Description, Price) VALUES (15, "Stamps", "Antique piece of history", 2.99);

/* Build machines, inventory, and inventory items */
INSERT INTO MachineInventory(Id) VALUES (1000);
INSERT INTO MachineInventory(Id) VALUES (2000);

INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (1, 1000, 1, 100);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (2, 1000, 3, 50);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (3, 1000, 40, 50);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (4, 1000, 7, 0);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (5, 1000, 16, 3);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (6, 1000, 15, 0);

INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (7, 2000, 1, 100);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (8, 2000, 3, 50);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (9, 2000, 40, 50);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (10, 2000, 7, 0);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (11, 2000, 16, 3);
INSERT INTO MachineInventoryLineItem(Id, MachineInventoryId, ItemId, Quantity) VALUES (12, 2000, 15, 0);

INSERT INTO Machine(Id, MachineInventoryId) VALUES (10, 1000);
INSERT INTO Machine(Id, MachineInventoryId) VALUES (20, 2000);

/* Build transaction history */
INSERT INTO "Transaction"(Id, AmountTendered) VALUES (1, 20.00);
INSERT INTO "Transaction"(Id, AmountTendered) VALUES (2, 900.76);

INSERT INTO TransactionLineItem(Id, TransactionId, MachineId, ItemId, Quantity) VALUES (1, 1, 1000, 1, 1); /* transaction 1, machine 1, item 1, quanitty 2 */
INSERT INTO TransactionLineItem(Id, TransactionId, MachineId, ItemId, Quantity) VALUES (2, 1, 1000, 7, 1); /* transaction 1, machine 1, item 7, quanitty 1 */
INSERT INTO TransactionLineItem(Id, TransactionId, MachineId, ItemId, Quantity) VALUES (3, 2, 2000, 9, 1); /* transaction 2, machine 2, item 9, quanitty 7 */