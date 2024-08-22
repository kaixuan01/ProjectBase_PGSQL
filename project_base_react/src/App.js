import './CSS/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router } from 'react-router-dom';
import MySidebar from './Control/MySidebar';
import { MyPageContainer } from './Control/MyPageContainer';
import Login from './View/Login';
import MyTopBar from './Control/MyTopBar';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { initData } from './Redux/actions';

function App() {
  const dispatch = useDispatch();

  useEffect(() => {
    // Load 'isLogin' from localStorage if it exists
    const storedLoginStatus = localStorage.getItem('isLogin');
    dispatch(initData('isLogin', storedLoginStatus || 'NotLogin'));
  }, [dispatch]);

  const isLogin = useSelector((state) => state.isLogin);

  useEffect(() => {
    // Persist 'isLogin' to localStorage whenever it changes
    if (isLogin) {
      localStorage.setItem('isLogin', isLogin);
    }
  }, [isLogin]);

  return (
    <div className='app-container'>
      {isLogin === 'Login' ? (
        <Router>
          <div className="app-container">
            <MyTopBar />
            <div className="d-flex">
              <div className="my-sidebar">
                <MySidebar />
              </div>
              <div className="my-page-container">
                <MyPageContainer />
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
