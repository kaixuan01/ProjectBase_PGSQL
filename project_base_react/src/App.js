import './CSS/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router } from 'react-router-dom';
import MySidebar from './Control/MySidebar';
import { MyPageContainer } from './Control/MyPageContainer';
import Login from './View/login';
import MyTopBar from './Control/MyTopBar';
import { useEffect, useState, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { initData, updateData, deleteData } from './Redux/actions';
function App() {

  const dispatch = useDispatch();

  useEffect(() => {
    // localStorage.setItem('isLogin', isLogin);
    dispatch(initData('isLogin', 'NotLogin'));
  }, []);
  var isLogin = useSelector((state) => state.isLogin);
  console.log(isLogin);
  return (
    <div className='app-container'>
      {isLogin === 'Login' ? (
        <Router>
          <div className="app-container">
            <MyTopBar/>
            <div className="d-flex">
              <div className="my-sidebar">
                <MySidebar/>
              </div>
              <div className="my-page-container">
                <MyPageContainer/>
              </div>
            </div>
          </div>
        </Router>
      ) : (
        <Login />
      )}
    </div>
  );
}

export default App;
