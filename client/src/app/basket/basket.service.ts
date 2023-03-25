import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AcntService } from '../account/acnt.service';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = 'https://localhost:9010';
  constructor(private http: HttpClient, private acntService: AcntService, private router: Router) { }
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();
  private basketTotal = new BehaviorSubject<IBasketTotal | null>(null);
  basketTotal$ = this.basketTotal.asObservable();

  getBasket(username: string){
    return this.http.get<IBasket>(this.baseUrl+'/Basket/GetBasket/rahul').subscribe({
      //update the basketsource so that via observable these values will be available to the subscribers via component
      next:basket=>{
        this.basketSource.next(basket);
        this.calculateBasketTotal();
      }
    });
  }
  setBasket(basket: IBasket){
    return this.http.post<IBasket>(this.baseUrl +'/Basket/CreateBasket', basket).subscribe({
      next: basket =>{
        this.basketSource.next(basket);
        this.calculateBasketTotal();
      }
    });
  }

  checkoutBasket(basket: IBasket){
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': this.acntService.authorizationHeaderValue
      })
    };
    return this.http.post<IBasket>(this.baseUrl +'/Basket/CheckoutV2', basket, httpOptions).subscribe({
      next: basket =>{
        this.basketSource.next(null);
        this.router.navigateByUrl('/');
      }
    });
  }

  getCurrentBasket(){
    return this.basketSource.value;
  }

  // Basket operations starts here
  addItemToBasket(item: IProduct, quantity = 1){
    const itemToAdd :IBasketItem = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    //now items can be added in the basket
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const founItemIndex = basket.items.findIndex((x)=>x.productId === item.productId);
    basket.items[founItemIndex].quantity++;
    this.setBasket(basket);
  }

  removeItemFromBasket(item:IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    if(basket.items.some((x) =>x.productId ===item.productId)){
      basket.items = basket.items.filter((x)=>x.productId!== item.productId)
      if(basket.items.length>0){
        this.setBasket(basket);
      }else{
        this.deleteBasket(basket.userName);
      }
    }
  }
  deleteBasket(userName: string) {
    return this.http.delete(this.baseUrl + '/Basket/DeleteBasket/' + userName).subscribe({
      next:(response) =>{
        this.basketSource.next(null);
        this.basketTotal.next(null);
        localStorage.removeItem('basket_username');
      }, error: (err)=>{
        console.log('Error Occurred while deletin basket');
        console.log(err);
      }
    })
  }

  decrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const founItemIndex = basket.items.findIndex((x)=>x.productId === item.productId);
    if(basket.items[founItemIndex].quantity >1){
      basket.items[founItemIndex].quantity--;
      this.setBasket(basket);
    }else {
      this.removeItemFromBasket(item);
    }
  }

  // Basket operations ends here

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
     //if we have the item in basket which matches the Id, then we can get here
     const item = items.find(x=>x.productId == itemToAdd.productId);
     if(item){
      item.quantity +=quantity;
     }else{
      itemToAdd.quantity = quantity;
      //then add the items in the basket
      items.push(itemToAdd);
     }
     return items;
  }

  private createBasket(): Basket {
    //since we have created class
    const basket = new Basket();
    localStorage.setItem('basket_username', 'rahul'); //TODO: rahul can be replaced with LoggedIn User
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct): IBasketItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      imageFile: item.imageFile,
      quantity:0
    }
  }

  private calculateBasketTotal(){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    //We are going to loop over in array and calculate total
    const total = basket.items.reduce((x, y)=> (y.price * y.quantity) + x, 0);
    this.basketTotal.next({total});
  }

}
