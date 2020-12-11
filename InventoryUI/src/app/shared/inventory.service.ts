import { Injectable } from '@angular/core';
import {Product} from './product.model';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import{environment} from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

selectedProduct:Product;
  formData  : Product;
  list : Product[];
  readonly rootURL =environment.apiUrl+"/api/inventory";

  constructor(private http : HttpClient,private router:Router,private toastr:ToastrService) { }

  postProduct(formData : any){
   
   return this.http.post<any>(this.rootURL,formData);
    
  }

  refreshList(){
    this.http.get(this.rootURL).subscribe(
      data=>{
        console.log(data);
        this.list=data as Product[];
        this.list.forEach(element => {
          element.Thumbnail="data:image/jpeg;base64,"+element.Thumbnail;
        });
      }
    )
    
    console.log(this.list);
  }

  GetProduct(id:number){
    this.http.get(this.rootURL+"/"+id).subscribe(
      data=>{
        console.log(data);
        this.selectedProduct=data as Product;
        
          this.selectedProduct.ProductImage="data:image/jpeg;base64,"+this.selectedProduct.ProductImage;
          this.router.navigateByUrl('Products/Details')
      }
    )}

   deleteProduct(id : number){
     this.http.delete(this.rootURL+"/"+id).subscribe(
       data=>{
         this.refreshList();
      this.toastr.error("Deleted Succesfully", "Product");

       }
     )
   }
}
