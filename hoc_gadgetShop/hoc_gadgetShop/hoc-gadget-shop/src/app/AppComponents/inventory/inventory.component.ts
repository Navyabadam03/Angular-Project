import { Component, inject, Type } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogBoxComponent } from '../../AppComponent/dialog-box/dialog-box.component';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.css'
})
export class InventoryComponent {
  httpClient = inject(HttpClient)
  productIdToDelete: number = 0;
  private modalService = inject(NgbModal)

  inventoryData = {
    productId: "",
    productName: "",
    availableQty: 0,
    reorderPoint: 0
  }

  disableProductIdInput = false;

  inventoryDetails: any;

  ngOnInit() {
    this.getInventoryDetails();
  }

  getInventoryDetails() {
    let apiUrl = "https://localhost:7019/api/Inventory";

    this.httpClient.get(apiUrl).subscribe(data => {
      this.inventoryDetails = data;
      console.log(this.inventoryDetails);
    });

    this.inventoryData = {
      productId: "",
      productName: "",
      availableQty: 0,
      reorderPoint: 0
    };
    this.disableProductIdInput = false;
  }

  onSubmit(): void {
    let apiUrl = "https://localhost:7019/api/Inventory";

    let httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'my-auth-token',
        'Content-Type': "application/json"
      })
    }

    if (this.disableProductIdInput == true) {
      this.httpClient.put(apiUrl, this.inventoryData, httpOptions).subscribe({
        next: v => console.log(v),
        error: e => console.log(e),
        complete: () => {
          alert("Form Submitted successfully :" + JSON.stringify(this.inventoryData));
          this.getInventoryDetails();
        }
      });
    }
    else {
      this.httpClient.post(apiUrl, this.inventoryData, httpOptions).subscribe({
        next: v => console.log(v),
        error: e => console.log(e),
        complete: () => {
          alert("Form Submitted successfully :" + JSON.stringify(this.inventoryData));
          this.getInventoryDetails();
        }
      });
    }
  }

  openConfirmDialog(productId: number) {
    this.productIdToDelete = productId;
    console.log(this.productIdToDelete);
    this.modalService.open(DialogBoxComponent).result.then(data => {
      console.log(data);
      if (data.event == "confirm") {
        this.deleteInventory();
      }
    });
  }

  deleteInventory(): void {
    let apiUrl = "https://localhost:7019/api/Inventory?productId=" + this.productIdToDelete;

    this.httpClient.delete(apiUrl).subscribe(data => {
      this.getInventoryDetails();
    });
  }

  populateFormForEdit(inventory: any) {
    this.inventoryData.productId = inventory.ProductId;
    this.inventoryData.productName = inventory.ProductName;
    this.inventoryData.availableQty = inventory.AvailableQty;
    this.inventoryData.reorderPoint = inventory.ReOrderPoint;

    this.disableProductIdInput = true;
  }
}
