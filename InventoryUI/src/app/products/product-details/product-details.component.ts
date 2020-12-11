import { Component, OnInit } from '@angular/core';
import { InventoryService } from 'src/app/shared/inventory.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  constructor(public service:InventoryService,private router: Router ) { }

  ngOnInit(): void {
  }
  backClicked() {
    this.router.navigateByUrl('');
  }
}
