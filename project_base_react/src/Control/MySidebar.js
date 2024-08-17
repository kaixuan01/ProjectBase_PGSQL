import { useLocation } from 'react-router-dom';
import myURLRoutes from '../Comon/RoutePath';
import { Sidebar, Menu, MenuItem, SubMenu } from 'react-pro-sidebar';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';
import { useState } from 'react';

export default function MySidebar({ setIsLogin }) {
    const [collapse, setCollapse] = useState(false);
    const location = useLocation();

    const handleLogout = () => {
        localStorage.removeItem('isLogin');
        setIsLogin(false);
    };

    return (
        <Sidebar
            style={{ height: "100vh", display: 'flex', flexDirection: 'column', justifyContent: 'space-between' }}
            collapsed={collapse}
            backgroundColor='#2E3B4E'
        >
            <div>
                <Menu
                    menuItemStyles={{
                        root: {
                            fontSize: '16px',
                            color: '#BBB',
                            transition: 'color 0.3s, background-color 0.3s',
                            fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif", // Font style
                        },
                        icon: {
                            color: '#FFF',
                            fontSize: '18px',
                        },
                        label: {
                            fontWeight: 'bold',
                        },
                        subMenuContent: {
                            backgroundColor: '#444d5c',
                        },
                        button: ({ level, active }) => {
                            let styles = {
                                ':hover': {
                                    backgroundColor: '#3D4F6A',
                                    color: '#FFF', // Hover color
                                },
                            };

                            if (active) {
                                styles.backgroundColor = '#1C2A3E';
                                styles.color = '#FFF';
                            }

                            if (level === 0) {
                                styles = {
                                    ...styles,
                                    ':hover': {
                                        backgroundColor: '#3D4F6A',
                                    },
                                };
                            }
                            return styles;
                        },
                    }}
                >
                    <MenuItem
                        icon={
                            <FontAwesomeIcon
                                icon={faBars}
                                style={{
                                    transform: collapse ? 'rotate(90deg)' : 'rotate(0deg)',
                                    transition: 'transform 0.3s ease'
                                }}
                            />}
                        onClick={() => setCollapse(!collapse)}
                    >
                    </MenuItem>
                    {myURLRoutes.map((route, index) => (
                        route.excludedMenu ? null :
                            route.subRoutes && route.subRoutes.length > 0 ? (
                                <SubMenu icon={route.icon} key={index} label={route.name} disabled={route.excludedMenu}>
                                    {route.subRoutes.map((subRoute, subIndex) => (
                                        route.excludedMenu ? null :
                                            <MenuItem
                                                icon={subRoute.icon}
                                                key={subIndex}
                                                active={location.pathname === `${route.path}${subRoute.path}`}
                                                component={<Link to={`${route.path}${subRoute.path}`} />}
                                            >
                                                {subRoute.name}
                                            </MenuItem>
                                    ))}
                                </SubMenu>
                            ) : (
                                <MenuItem
                                    active={location.pathname === route.path}
                                    icon={route.icon}
                                    key={index}
                                    component={<Link to={route.path} />}
                                    disabled={route.excludedMenu}
                                >
                                    {route.name}
                                </MenuItem>
                            )
                    ))}
                </Menu>
            </div>

        </Sidebar>
    );
}
