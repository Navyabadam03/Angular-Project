import { Component, inject } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerDialogBoxComponent } from '../customer-dialog-box/customer-dialog-box.component';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { DialogBoxComponent } from '../../AppComponent/dialog-box/dialog-box.component';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent {
  private modalService = inject(NgbModal)

  httpClient= inject(HttpClient)

  customerDetails:any;

  openCustomerDialog(){
      this.modalService.open(CustomerDialogBoxComponent).result.then(data=>{
        if(data.event=="closed"){
          this.getCustomerDetails();
        }
      });
  }

  ngOnInit()
  {
    this.getCustomerDetails();
  }

  getCustomerDetails(){
      let apiUrl="https://localhost:7019/api/Customer";

      this.httpClient.get(apiUrl).subscribe(result=>{
        this.customerDetails=result;
        console.log(this.customerDetails);
      })
  }

  openConfirmDialog(customerId: any){
    this.modalService.open(DialogBoxComponent).result.then(data => {
      console.log(data);
      if (data.event == "confirm") {
       this.deleteCustomerDetails(customerId);
      }
    });
  }

  deleteCustomerDetails(customerId: any){
    let apiUrl="https://localhost:7019/api/Customer?customerId=";

    this.httpClient.delete(apiUrl+customerId).subscribe(data=>{
      this.getCustomerDetails();
    });

  }
  
  openEditDialogBox(customer: any){
    const modalReference = this.modalService.open(CustomerDialogBoxComponent);
    modalReference.componentInstance.customer = {
      customerId : customer.CustomerId,
      firstName: customer.FirstName,
      lastName: customer.LastName,
      email: customer.Email,
      registrationDate: customer.RegistrationDate,
      phone: customer.Phone
    };
    modalReference.result.then(data=>{
      if(data.event=="closed"){
        this.getCustomerDetails();
      }
    });
  }

}
