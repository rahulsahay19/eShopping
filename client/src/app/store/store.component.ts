import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { StoreParams } from '../shared/models/storeParams';
import { IType } from '../shared/models/type';
import { StoreService } from './store.service';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss']
})
export class StoreComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] =[];
  storeParams = new StoreParams();
  totalCount = 0;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Ascending', value: 'priceAsc' },
    { name: 'Price: Descending', value: 'priceDesc'}
  ];

  constructor(private storeService: StoreService){}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }
  getProducts(){
    this.storeService.getProducts(this.storeParams).subscribe({
      next: response =>{
        this.products = response.data;
        this.storeParams.pageNumber = response.pageIndex;
        this.storeParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: error =>console.log(error)
    });
  }
  getBrands(){
    this.storeService.getBrands().subscribe({
      next: response =>{
        this.brands = [{id:'', name:'All'}, ...response]
      },
      error: error =>console.log(error)
    });
  }
  getTypes(){
    this.storeService.getTypes().subscribe({
      next: response =>{
        this.types = [{id:'', name:'All'}, ...response]
      },
      error: error =>console.log(error)
    });
  }
  onBrandSelected(brandId: string){
    this.storeParams.brandId = brandId;
    this.getProducts();
  }
  onTypeSelected(typeId: string){
    this.storeParams.typeId = typeId;
    this.getProducts();
  }

  onSortSelected(sort:string){
    this.storeParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event:any){
    this.storeParams.pageNumber = event.page;
    this.getProducts();
  }

  onSearch(){
    this.storeParams.search = this.searchTerm?.nativeElement.value;
    this.storeParams.pageNumber = 1;
    this.getProducts();
  }

  onReset(){
    if(this.searchTerm){
      this.searchTerm.nativeElement.value = '';
      this.storeParams = new StoreParams();
      this.getProducts();
    }
  }

}
