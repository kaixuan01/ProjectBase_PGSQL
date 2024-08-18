import './CSS/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router } from 'react-router-dom';
import MySidebar from './Control/MySidebar';
import { MyPageContainer } from './Control/MyPageContainer';
import Login from './View/login';
import MyTopBar from './Control/MyTopBar';
import { useEffect, useState, useCallback } from 'react';

function App() {
  const [isLogin, setIsLogin] = useState(() => {
    return localStorage.getItem('isLogin') === 'true';
  });

  useEffect(() => {
    localStorage.setItem('isLogin', isLogin);
  }, [isLogin]);

  const handleLogin = useCallback(() => {
    setIsLogin(true);
  }, []);

  const handleLogout = useCallback(() => {
    setIsLogin(false);
  }, []);

  return (
    <div className='app-container'>
      {isLogin ? (
        <Router>
          <div className="app-container">
            <MyTopBar onLogout={handleLogout} />
            <div className="d-flex">
              <div className="my-sidebar">
                <MySidebar setIsLogin={setIsLogin} />
              </div>
              <div className="my-page-container">
                <MyPageContainer />
              </div>
            </div>
          </div>
        </Router>
      ) : (
        <Login onLogin={handleLogin} />
      )}
    </div>
  );
}

export default App;
