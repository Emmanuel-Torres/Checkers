import React from 'react'
import Login from '../components/auth/Login'
import Logout from '../components/auth/Logout';
import { useStoreSelector } from '../store';

export default function LoginView() {
  var userToken = useStoreSelector((state) => state.auth.userToken);
  return (
    <div className='container border border-dark border-5 rounded p-3 my-2 shadow text-center'>
      {
        userToken === undefined?
          <>
            <h1>Login</h1>
            <Login />
          </> :
          <>
            <h1>Log Out</h1>
            <Logout />
          </>
      }

    </div>
  )
}
