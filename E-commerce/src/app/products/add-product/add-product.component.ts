import { Component, OnInit } from '@angular/core';
import{InventoryService} from 'src/app/shared/inventory.service';
import { NgForm } from '@angular/forms';
import { Product } from 'src/app/shared/product.model';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  constructor(public service:InventoryService,private toastr:ToastrService) { }
  base64textString:string;
  selectedImage:File;
  product:Product;
  InsertForm: FormData=null;
  ngOnInit(): void {
    this.resetForm();
  }
  resetForm(form?: NgForm) {
    if (form != null)
      form.resetForm();
    this.service.formData = {
      Id: null,
      Name: '',
      Description: '',
      Imagefile:null,
      Price:null,
      Thumbnail:'',
      ProductImage:''
      
    }
  }
  processFile(file: FileList){
  
    this.selectedImage=file.item(0);

}



_handleReaderLoaded(readerEvt) {
   var binaryString = readerEvt.target.result;
          this.base64textString= btoa(binaryString);
          console.log(this.base64textString);
       
  }

  onSubmit(form: NgForm) {
    this.InsertForm=new FormData();
    this.InsertForm.append("Name",form.value.Name);
    this.InsertForm.append("Description",form.value.Description);
    this.InsertForm.append("Price",form.value.Price);
    this.InsertForm.append("ImageFile",this.selectedImage,this.selectedImage.name)
    this.product=form.value;
    this.product.Imagefile=this.selectedImage;
    this.service.postProduct(this.InsertForm).subscribe(res => {
      this.resetForm(form);
      this.service.refreshList();
      this.toastr.success("Added Succesfully", "Product");
    });
  }


}
