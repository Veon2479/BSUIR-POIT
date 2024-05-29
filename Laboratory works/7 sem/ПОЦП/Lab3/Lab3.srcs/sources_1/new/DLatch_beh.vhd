----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 06:50:28 PM
-- Design Name: 
-- Module Name: DLatch_beh - Behavioral
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

entity DLatch_beh is
    Port ( D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
end DLatch_beh;

architecture Behavioral of DLatch_beh is
    attribute dont_touch : string;
    attribute dont_touch of Behavioral : architecture is "true";
begin
    Main: Process(D)
    begin
        Q <= D;
        NQ <= not D;             
    end process;
end Behavioral;
