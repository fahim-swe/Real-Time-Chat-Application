import { Injectable } from '@angular/core';
import { message } from '@common/models/message';
import { sendmessage } from '@common/models/sendmessage';
import { faL } from '@fortawesome/free-solid-svg-icons';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = "http://localhost:5276/hubs";
  token : string;
  userId : string;

  private hubConnection : HubConnection;
  public messageThreadSource = new BehaviorSubject<message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  checkNew = false;

  constructor() {

    let access_token = localStorage.getItem('access_token');
    if(access_token != null){
      this.token;
    }
   }

  createHubConnection(recipientUserId: string)
  {

    let access_token = localStorage.getItem('access_token');
    let id = localStorage.getItem("user_id");
    
    if(access_token != null && id != null){
      this.token = access_token;
      this.userId = id;
    }

    this.hubConnection = new HubConnectionBuilder()
        .withUrl('https://localhost:7145/hubs/message?user=' + recipientUserId , {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets,
          accessTokenFactory: ()=> this.token
        })
        .withAutomaticReconnect()
        .build()


    this.hubConnection.start().catch(erorr => console.log(erorr));

    this.hubConnection.on('RecieveMessageThread', message => {
      this.messageThreadSource.next(message);
    })

    this.hubConnection.on('NewMessage', message => {
      
      this.messageThreadSource.pipe(take(1)).subscribe(messages => {
      this.messageThreadSource.next([...messages, message]);
        this.checkNew = true;
      })  
    })
  }

  stopHubConnection()
  {
    if(this.hubConnection){
      this.messageThreadSource.next([]);
      this.hubConnection.stop();
    }
  }

  async sendMessage(message: any)
  {
    
    return this.hubConnection.invoke("SendMessage", message)
         .catch(erorr => {
          console.log("Error in sending sms " + erorr);
         });
  }
}
