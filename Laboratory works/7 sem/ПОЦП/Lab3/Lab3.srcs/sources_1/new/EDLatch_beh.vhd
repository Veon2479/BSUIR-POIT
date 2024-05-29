----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 07:21:37 PM
-- Design Name: 
-- Module Name: EDLatch_beh - Behavioral
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

entity EDLatch_beh is
    Port ( E : in STD_LOGIC;
           D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
end EDLatch_beh;

architecture Behavioral of EDLatch_beh is
    signal v : std_logic;
begin
    Main: process(D, E)
    begin
        if E = '1' then
            v <= D;
        end if;     
    end process;
    
    Q <= v;
    NQ <= not v;

end Behavioral;
