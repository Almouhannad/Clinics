import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';
import * as config from '../../../../config'
import { GetAllDoctorUsersResponse } from '../../classes/usecases/admin-usecases/list-doctor-users/get-all-doctor-users-response';
import { GetAllDoctorUsersResult } from '../../classes/usecases/admin-usecases/list-doctor-users/get-all-doctor-users-result';
import { CreateDoctorUserCommand } from '../../classes/usecases/admin-usecases/create-doctor-user/create-doctor-user-command';
import { CreateDoctorUserResult } from '../../classes/usecases/admin-usecases/create-doctor-user/create-doctor-user-result';
import { DoctorUserResponse } from '../../classes/usecases/admin-usecases/list-doctor-users/doctor-user-response';
import { UpdateDoctorPersonalDataQuery } from '../../classes/usecases/admin-usecases/update-doctor-user/update-doctor-user-personal-data/update-doctor-personal-data-query';
import { UpdateDoctorPersonalDataResult } from '../../classes/usecases/admin-usecases/update-doctor-user/update-doctor-user-personal-data/update-doctor-personal-data-result';
import { UpdateDoctorUserDataResult } from '../../classes/usecases/admin-usecases/update-doctor-user/update-doctor-user-account-data/update-doctor-user-data-result';
import { UpdateDoctorUserQuery } from '../../classes/usecases/admin-usecases/update-doctor-user/update-doctor-user-account-data/update-doctor-user-query';


@Injectable({
  providedIn: 'root'
})
export class DoctorUsersService {

  // #region DI CTOR
  constructor(private http: HttpClient) { }
  // #endregion

  // #region Constants
  private readonly DOCTORUSERS_ENDPOINT: string = `${config.apiUrl}/Users/Doctors`;
  // #endregion

  // #region Methods

  // #region Get all doctor users
  getDoctorUsers(): Observable<GetAllDoctorUsersResult> {
    return this.http
    .get<GetAllDoctorUsersResponse>(this.DOCTORUSERS_ENDPOINT)
    .pipe(
      map((getAllDoctorUsersResponse: GetAllDoctorUsersResponse) => {
        return new GetAllDoctorUsersResult(true, '', getAllDoctorUsersResponse.doctorUsers);
      }),
      catchError((error: HttpErrorResponse) => {
        return of(new GetAllDoctorUsersResult(false, error.error.detail))
      })
    )
  }
  // #endregion
  
  // #region Create doctor user
  createDoctorUser(command: CreateDoctorUserCommand) : Observable<CreateDoctorUserResult> {
    return this.http.post(this.DOCTORUSERS_ENDPOINT, command)
    .pipe(
      map (_ => {
        return new CreateDoctorUserResult(true);
      }),
      catchError((error: HttpErrorResponse) => {
        return of(new CreateDoctorUserResult(false, error.error.detail))
      })
    );
  }
  // #endregion

  // #region Get doctor user by Id
  getDoctorUserById(id: number): Observable<DoctorUserResponse | null>
  {
    return this.http.get<DoctorUserResponse>(`${this.DOCTORUSERS_ENDPOINT}/${id}`)
    .pipe(
      map((doctorUser: DoctorUserResponse) => {
        return doctorUser;
      }),
      catchError((error: HttpErrorResponse) => {
        return of(null);
      })
    )
  }
  // #endregion
  
  // #region Update doctor peronsal info
  updateDoctorPersonalInfo(query: UpdateDoctorPersonalDataQuery) : Observable<UpdateDoctorPersonalDataResult> {
    return this.http.put(this.DOCTORUSERS_ENDPOINT, query)
    .pipe(
      map(_ => {
        return new UpdateDoctorPersonalDataResult(true);
      }),
      catchError((error: HttpErrorResponse) => {
        return of (new UpdateDoctorPersonalDataResult(false, error.error.detail))
      })
    )
  }
  
  // #endregion

  // #region Update doctor user data 
  updateDoctorUserData(query: UpdateDoctorUserQuery): Observable<UpdateDoctorUserDataResult> {
    return this.http.put(`${this.DOCTORUSERS_ENDPOINT}/Users`, query)
    .pipe(
      map (_ => {
        return new UpdateDoctorUserDataResult(true);
      }),
      catchError((error: HttpErrorResponse) => {
        return of (new UpdateDoctorUserDataResult(false, error.error.detail))
      })
    )
  }  
  // #endregion

  // #endregion
}