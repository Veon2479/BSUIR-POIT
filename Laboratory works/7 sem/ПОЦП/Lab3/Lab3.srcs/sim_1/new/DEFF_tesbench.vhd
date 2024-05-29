----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:21:13 PM
-- Design Name: 
-- Module Name: DEFF_tesbench - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity DEFF_tesbench is
--  Port ( );
end DEFF_tesbench;

architecture Behavioral of DEFF_tesbench is
    Component DEFF is 
        Port(
        d, e, clk: in std_logic; 
        q: out std_logic
    );

    end component;
       function to_stdlogic(i: integer) return std_logic is
    begin
        case i is
            when 0 => return '0';
            when 1 => return '1';
            when others => return 'X';
        end case;
    end function;
   
   signal d, e, clk, q: std_logic := '0';
    
begin

u: DEFF port map(d=>d, e=>e, clk=>clk, q=>q);

Main: process
begin
    for o in 0 to 1 loop 
        for i in 0 to 1 loop
            for k in 0 to 1 loop
                d<=to_stdlogic(i);
                e <=to_stdlogic(k);
                wait for 10 ns;
            end loop;
        end loop;
    end loop;
    wait;
end process;

Clkp: process
begin
        clk<='0';
        wait for 10 ns;
        clk <= '1';
        wait for 10 ns;

end process;
end Behavioral;
