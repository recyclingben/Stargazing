import React, { Component } from "react";
import { NavigationBar} from "../components/NavigationBar/NavigationBar";
import { NavigationBarOption } from "../components/NavigationBar/NavigationBarOption";


class Layout extends Component {
    constructor(props) {
        super(props);

        this.navbar = React.createRef();
        
        this.closeDropdown = this.closeDropdown.bind(this);
    }

    closeDropdown() {
        this.navbar.closeDropdown();
    }

    render() {
        return (
            <div id="Wrapper">
                <NavigationBar title={"stargazing"} titlePage={"/"} wrappedComponentRef={c => { this.navbar = c; }}>
                    <NavigationBarOption page={"/"} onClick={this.closeDropdown}>home</NavigationBarOption>
                    <NavigationBarOption page={"/about"} onClick={this.closeDropdown}>about</NavigationBarOption>
                </NavigationBar>
                { this.props.children }
            </div>
        );
    }
}
export { Layout };