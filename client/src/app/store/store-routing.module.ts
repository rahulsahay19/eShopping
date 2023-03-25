import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreComponent } from './store.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

const routes:Routes = [
  {path:'', component: StoreComponent},
  {path:':id', component: ProductDetailsComponent, data:{breadcrumb:{alias:'productDetails'}}}
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[
    RouterModule
  ]
})
export class StoreRoutingModule { }
