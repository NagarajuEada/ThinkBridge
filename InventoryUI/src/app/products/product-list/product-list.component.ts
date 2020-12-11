import { Component, OnInit } from '@angular/core';
import { InventoryService } from 'src/app/shared/inventory.service';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';
import { Product } from 'src/app/shared/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  constructor(public service:InventoryService,private router:Router ) { }

  ngOnInit(): void {
    this.service.refreshList();

  }
  SelectProduct(product:Product)
  {
    console.log(product);
   this.service.GetProduct(product.Id);
  }
  deleteProduct(product:Product){
    this.service.deleteProduct(product.Id);
  }
}
